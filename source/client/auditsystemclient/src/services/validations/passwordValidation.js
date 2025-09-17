const validatePassword = (password) => {
  // Sanitize the input to remove HTML tags and JavaScript code
  const sanitizedPassword = password.replace(/(<([^>]+)>)/gi, "");

  if (!sanitizedPassword) return 'Password is required';
  if (sanitizedPassword.length < 8) return 'Password must be at least 8 characters';
  if (!/[A-Z]/.test(sanitizedPassword)) return 'Password must contain an uppercase letter';
  if (!/[0-9]/.test(sanitizedPassword)) return 'Password must contain a number';
  if (!/[^A-Za-z0-9]/.test(sanitizedPassword))
    return 'Password must contain a special character';
  return null;
};

export default validatePassword;
