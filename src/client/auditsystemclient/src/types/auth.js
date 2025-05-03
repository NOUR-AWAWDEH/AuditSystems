// src/types/auth.js
export const LoginResponseDto = {
  jwtToken: '',
  refreshToken: '',
  isEmailVerified: false
};

export const LoginRequestDto = {
  username: '',
  password: ''
};
