// validateRole.js
export const isNotAdmin = (user) => {
  if (!user || typeof user.role !== "string") {
    return false;
  }

  return user.role.toLowerCase() !== "admin";
};

export const requireNotAdmin = (user) => {
  if (!isNotAdmin(user)) {
    throw new Error("Admin users are not allowed");
  }

  return true;
};