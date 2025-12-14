// validateEmail.js
const HTML_TAG_REGEX = /(<([^>]+)>)/gi;

const EMAIL_REGEX =
  /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

export function validateEmail(email) {
  if (typeof email !== "string") return false;

  const sanitized = email.replace(HTML_TAG_REGEX, "");
  const trimmed = sanitized.trim();

  return EMAIL_REGEX.test(trimmed);
}
