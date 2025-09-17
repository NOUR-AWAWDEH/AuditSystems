export function validateEmail(email) {
  // Sanitize the input to remove HTML tags and JavaScript code
  const sanitizedEmail = email.replace(/(<([^>]+)>)/gi, "");
  const trimmedEmail = sanitizedEmail.trim();
  const emailRegex = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
  return emailRegex.test(trimmedEmail);
}
