export function validateUsername(username) {
  // Sanitize the input to remove HTML tags and JavaScript code
  const sanitizedUsername = username.replace(/(<([^>]+)>)/gi, "");

  if (!sanitizedUsername) return 'Username is required';
  if (sanitizedUsername.length < 3) return 'Username must be at least 3 characters';
  if (sanitizedUsername.length > 50) return 'Username must be less than 50 characters';
  if (!/^[a-zA-Z0-9_.-]+$/.test(sanitizedUsername))
    return 'Username can only contain letters, numbers, underscores, dots, and hyphens';
  return null;
}
