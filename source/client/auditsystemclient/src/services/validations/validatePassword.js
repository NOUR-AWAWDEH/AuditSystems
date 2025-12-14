// validatePassword.js
const HTML_TAG_REGEX = /(<([^>]+)>)/gi;

export default function validatePassword(password) {
  if (typeof password !== "string") return "Password is required";

  const sanitized = password.replace(HTML_TAG_REGEX, "").trim();

  if (!sanitized) return "Password is required";
  if (sanitized.length < 8) return "Password must be at least 8 characters";
  if (!/[A-Z]/.test(sanitized)) return "Password must contain an uppercase letter";
  if (!/[0-9]/.test(sanitized)) return "Password must contain a number";
  if (!/[^A-Za-z0-9]/.test(sanitized)) return "Password must contain a special character";

  return null;
}
