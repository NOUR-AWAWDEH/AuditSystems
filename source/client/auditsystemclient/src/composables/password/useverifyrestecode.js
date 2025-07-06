import { ref } from 'vue';

export function useVerifyResetCode() {
  const isVerified = ref(false);

  const handleVerificationSuccess = (response) => {
    isVerified.value = true;
    const resultData = response.data?.result || {}; // Safely access result
    return {
      success: true,
      data: resultData, // Use the safely accessed result
      message: response.successMessage,
    };
  };

  return {
    isVerified,
    handleVerificationSuccess
  };
}
