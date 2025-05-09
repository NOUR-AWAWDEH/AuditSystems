import api from '@/services/api/api';
import router from '@/router/router';

// Constants for localStorage keys
const TOKEN_KEY = 'token'; // Corrected key name
const REFRESH_TOKEN_KEY = 'refreshToken';

const REFRESH_INTERVAL = 55 * 60 * 1000; // 55 minutes in milliseconds
let refreshTokenTimeoutId = null;

const getLocalToken = () => localStorage.getItem(TOKEN_KEY); // Use the correct key here
const getLocalRefreshToken = () => localStorage.getItem(REFRESH_TOKEN_KEY);
const setLocalToken = (token) => localStorage.setItem(TOKEN_KEY, token);
const setLocalRefreshToken = (refreshToken) => localStorage.setItem(REFRESH_TOKEN_KEY, refreshToken);
const removeLocalTokens = () => {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(REFRESH_TOKEN_KEY);
};

const refreshToken = async () => {
    console.log('refreshToken called');
    const refreshTokenValue = getLocalRefreshToken();
    const currentToken = getLocalToken();
    console.log('refreshTokenValue:', refreshTokenValue);
    console.log('currentToken:', currentToken);
    if (!refreshTokenValue) {
        console.error('No refresh token available.');
        redirectToLogin();
        return false;
    }

    try {
        const response = await api.post(
            '/api/Auth/refresh-token',
            { refreshToken: refreshTokenValue },
            {
                headers: {
                    Authorization: `Bearer ${currentToken}`,
                },
            }
        );
        console.log('refreshToken response:', response); // Add this
        if (response.data.success && response.data.result) {
            const { jwtToken, refreshToken: newRefreshToken } = response.data.result;
            setLocalToken(jwtToken);
            setLocalRefreshToken(newRefreshToken);
            console.log('Token refreshed successfully:', response.data);
            scheduleRefreshToken();
            return true;
        } else {
            console.error('Failed to refresh token:', response.data);
            redirectToLogin();
            return false;
        }
    } catch (error) {
        console.error('Error during token refresh:', error);
        redirectToLogin();
        return false;
    }
};

const scheduleRefreshToken = () => {
    clearTimeout(refreshTokenTimeoutId);
    refreshTokenTimeoutId = setTimeout(refreshToken, REFRESH_INTERVAL);
};

const redirectToLogin = () => {
    removeLocalTokens();
    router.push('/login');
};

const initializeTokenRefresh = () => {
    console.log('initializeTokenRefresh called');
    const initialToken = getLocalToken();
    if (initialToken) {
        scheduleRefreshToken();
        console.log('Token refresh initialized. im working fine');
    }
};

export default {
    getLocalToken,
    setLocalToken,
    setLocalRefreshToken,
    removeLocalTokens,
    initializeTokenRefresh,
};
