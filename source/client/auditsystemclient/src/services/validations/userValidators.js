// userValidators.js
import validatePassword from "./validatePassword";
import { validateEmail } from "./validateEmail";
import { validateUsername } from "./validateUsername";
import { isNotAdmin } from "./validateRole";

const isValidGuid = (guid) => {
  const guidRegex = /^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$/i;
  return guidRegex.test(guid);
};

export const validateUserData = (userData) => {
  if (!userData) return "User data is required";

  // Username
  const usernameError = validateUsername(userData.userName || "");
  if (usernameError) return usernameError;

  // Email
  if (!userData.email?.trim()) return "Email is required";
  if (!validateEmail(userData.email)) return "Invalid email format";

  // Password
  const passwordError = validatePassword(userData.password || "");
  if (passwordError) return passwordError;
  if (userData.password !== userData.confirmPassword) return "Passwords do not match";

  // First & Last Name
  if (!userData.firstName?.trim()) return "First name is required";
  if (!userData.lastName?.trim()) return "Last name is required";

  // Role Name
  if (!userData.roleName?.trim()) return "Role name is required";
  if (!isNotAdmin({ role: userData.roleName })) return "Cannot assign Admin role to a new user";

  // Company ID (must be GUID)
  if (!userData.companyId) return "Company ID is required";
  if (!isValidGuid(userData.companyId)) return "Company ID must be a valid GUID";

  return null;
};
