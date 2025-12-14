export function validateUsername(username) {
  if (typeof username !== "string") {
    return "Username is required";
  }

  // Remove HTML tags
  const sanitized = username.replace(/(<([^>]+)>)/gi, "");

  // Do not allow leading spaces
  if (sanitized.startsWith(" ")) {
    return "Username cannot start with a space";
  }

  const trimmed = sanitized.trim();

  if (!trimmed) {
    return "Username is required";
  }

  if (trimmed.length < 4) {
    return "Username must be at least 4 characters";
  }

  if (trimmed.length > 50) {
    return "Username must be less than 50 characters";
  }

  const usernamePattern = /^[a-zA-Z0-9_.-]+$/;

  if (!usernamePattern.test(trimmed)) {
    return "Username can only contain letters, numbers, underscores, dots, and hyphens";
  }

  return null;
}