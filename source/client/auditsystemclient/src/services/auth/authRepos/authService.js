// authService.js
import api from '@/services/api/api';

const REFRESH_TOKEN_API_ENDPOINT = '/api/Auth/refresh-token';
const TOKEN_KEY = 'token';
const REFRESH_TOKEN_KEY = 'refreshToken';
const REFRESH_INTERVAL = 55 * 60 * 1000;
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

  if (!refreshTokenValue) {
    console.error('No refresh token available.');
    return false;
  }

  try {
    const { data } = await api.post(REFRESH_TOKEN_API_ENDPOINT, { refreshToken: refreshTokenValue });

    if (data.success && data.result) {
      const { jwtToken, refreshToken: newRefreshToken } = data.result;

      setLocalToken(jwtToken);
      setLocalRefreshToken(newRefreshToken);
      scheduleRefreshToken();
      return true;
    } else {
      console.error('Failed to refresh token:', data);
      return false;
    }
  } catch (error) {
    console.error('Error during token refresh:', error);
    return false;
  }
};


const scheduleRefreshToken = () => {
  clearTimeout(refreshTokenTimeoutId);
  refreshTokenTimeoutId = setTimeout(refreshToken, REFRESH_INTERVAL);
};

const initializeTokenRefresh = () => {
  if (getLocalToken()) {
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