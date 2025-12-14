import { validateEmail } from "./validateEmail";
import { validateUsername } from "./validateUsername";
import { isNotAdmin } from "./validateRole";

const isValidGuid = (guid) => {
    const guidRegex = /^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$/i;
    return guidRegex.test(guid);
};

export const validateUpdateUserData = (userData) => {
    if (!userData) return "User data is required";

    if (!userData.id) return "User ID is required";
    if (!isValidGuid(userData.id)) return "User ID must be a valid GUID";

    // Username
    const usernameError = validateUsername(userData.username || "");
    if (usernameError) return usernameError;

    // Email
    if (!userData.email?.trim()) return "Email is required";
    if (!validateEmail(userData.email)) return "Invalid email format";

    // First & Last Name
    if (!userData.firstName?.trim()) return "First name is required";
    if (!userData.lastName?.trim()) return "Last name is required";

    // Role
    if (!userData.roleName?.trim()) return "Role name is required";
    if (!isNotAdmin({ role: userData.roleName }))
        return "Cannot assign Admin role";

    return null;
};
