
export function useLoginValidation() {
  const validateUsername = (username) => {
    if (!username) return 'Username is required';
    if (username.length < 3) return 'Username must be at least 3 characters';
    if (username.length > 50) return 'Username must be less than 50 characters';
    if (!/^[a-zA-Z0-9_.-]+$/.test(username))
      return 'Username can only contain letters, numbers, underscores, dots, and hyphens';
    return null;
  };

  const validatePassword = (password) => {
    if (!password) return 'Password is required';
    if (password.length < 8) return 'Password must be at least 8 characters';
    if (!/[A-Z]/.test(password)) return 'Password must contain an uppercase letter';
    if (!/[0-9]/.test(password)) return 'Password must contain a number';
    if (!/[^A-Za-z0-9]/.test(password))
      return 'Password must contain a special character';
    return null;
  };

  return { validateUsername, validatePassword };
}
