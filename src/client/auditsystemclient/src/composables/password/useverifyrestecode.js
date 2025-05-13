function handleVerificationSuccess(response) {
  isVerified.value = true;
  const resultData = response.data?.result || {}; // Safely access result
  return {
    success: true,
    data: resultData, // Use the safely accessed result
    message: response.successMessage,
  };
}
