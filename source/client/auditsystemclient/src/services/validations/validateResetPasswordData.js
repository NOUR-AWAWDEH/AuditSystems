// src/services/validations/validateResetPasswordData.js

import validatePassword from "@/services/validations/validatePassword";

export function validateResetPasswordData(data) {
  if (!data || typeof data !== "object") {
    return "Invalid reset password payload";
  }

  const { email, token, newPassword, confirmNewPassword } = data;

  if (!email || typeof email !== "string") {
    return "Email is required";
  }

  if (!token || typeof token !== "string") {
    return "Reset token is required";
  }

  const passwordError = validatePassword(newPassword);
  if (passwordError) {
    return passwordError;
  }

  if (!confirmNewPassword || typeof confirmNewPassword !== "string") {
    return "Confirm password is required";
  }

  if (newPassword !== confirmNewPassword) {
    return "Passwords do not match";
  }

  return null;
}
