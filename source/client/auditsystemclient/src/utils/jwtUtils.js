/**
 * Decodes a JWT token to get its payload.
 * @param {string} token - The JWT token.
 * @returns {object|null} - The decoded payload or null if invalid.
 */
export function decodeToken(token) {
  try {
    const base64Url = token.split(".")[1];
    const base64 = base64Url.replace(/-/g, "+").replace(/_/g, "/");
    const jsonPayload = decodeURIComponent(
      atob(base64)
        .split("")
        .map((c) => "%" + ("00" + c.charCodeAt(0).toString(16)).slice(-2))
        .join("")
    );
    return JSON.parse(jsonPayload);
  } catch (e) {
    console.error("Error decoding token:", e);
    return null;
  }
}

/**
 * Checks if a JWT token is expired.
 * @param {string} token - The JWT token.
 * @returns {boolean} - True if expired or invalid, false otherwise.
 */
export function isTokenExpired(token) {
  if (!token) return true;

  const payload = decodeToken(token);
  if (!payload || !payload.exp) return true;

  const currentTime = Math.floor(Date.now() / 1000);
  return payload.exp < currentTime;
}
