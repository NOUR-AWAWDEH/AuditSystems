// validateEmail.js

const HTML_TAG_REGEX = /<[^>]*>/g;

const EMAIL_REGEX = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;

function sanitizeEmail(value) {
  return value.replace(HTML_TAG_REGEX, "").trim();
}

export function validateEmail(email) {
  if (typeof email !== "string") {
    return false;
  }

  const cleanedEmail = sanitizeEmail(email);

  if (cleanedEmail.length === 0) {
    return false;
  }

  return EMAIL_REGEX.test(cleanedEmail);
}
