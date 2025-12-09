
export const isNotAdmin = (user) => {
  if (!user || !user.role) {
    return false;
  }
  return user.role.toLowerCase() !== 'admin';
};

export const requireNotAdmin = (user) => {
  if (!isNotAdmin(user)) {
    throw new Error('Admin users are not allowed');
  }
};
