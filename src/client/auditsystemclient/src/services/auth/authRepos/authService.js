// src/services/auth/authRepos/authService.js
import api from '@/services/api/api';
import router from '@/router/router';

// Constants for localStorage keys
const TOKEN_KEY = 'token';
const REFRESH_TOKEN_KEY = 'refreshToken';
const REFRESH_INTERVAL = 55 * 60 * 1000; // 55 minutes
let refreshTokenTimeoutId = null;

const getLocalToken = () => localStorage.getItem(TOKEN_KEY);
const getLocalRefreshToken = () => localStorage.getItem(REFRESH_TOKEN_KEY);
const setLocalToken = (token) => localStorage.setItem(TOKEN_KEY, token);
const setLocalRefreshToken = (refreshToken) => localStorage.setItem(REFRESH_TOKEN_KEY, refreshToken);
const removeLocalTokens = () => {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(REFRESH_TOKEN_KEY);
};

const refreshToken = async () => {
    const refreshTokenValue = getLocalRefreshToken();
    const currentToken = getLocalToken();
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
        if (response.data.success && response.data.result) {
            const { jwtToken, refreshToken: newRefreshToken } = response.data.result;
            setLocalToken(jwtToken);
            setLocalRefreshToken(newRefreshToken);
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
    const initialToken = getLocalToken();
    if (initialToken) {
        scheduleRefreshToken();
    }
};

export default {
    getLocalToken,
    getLocalRefreshToken,
    setLocalToken,
    setLocalRefreshToken,
    removeLocalTokens,
    initializeTokenRefresh,
    refreshToken,
};
