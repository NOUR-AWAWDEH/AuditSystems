// src/services/supportingDocs/createSupportingDocumentRepository.js
import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';

const CREATE_SUPPORTING_DOC_ENDPOINT = 'api/SupportingDocs/create-supporting-document';

export const createSupportingDocumentRepository = {
  async create({ fileName, fileSize, url }) {
    try {
      const response = await api.post(CREATE_SUPPORTING_DOC_ENDPOINT, {
        fileName,
        fileSize,
        url,
      });

      if (response?.status === 201 && response?.data?.success) {
        return ApiResponseT.ok(response.data.result, response.data.successMessage);
      }

      const errors = Array.isArray(response?.data?.errors)
        ? response.data.errors
        : [{ message: response?.data?.message || 'Failed to create supporting document' }];

      return ApiResponseT.badRequest(errors, response?.data?.message);

    } catch (error) {
      console.error('Create Supporting Document Error:', error);
      let errorMessage = 'Failed to create supporting document';
      let errors = [{ message: errorMessage }];

      if (error?.response?.data) {
        errorMessage = error.response.data.message || errorMessage;
        errors = Array.isArray(error.response.data.errors)
          ? error.response.data.errors
          : [{ message: errorMessage }];
      } else if (error?.message) {
        errorMessage = error.message;
        errors = [{ message: errorMessage }];
      }

      return ApiResponseT.badRequest(errors, errorMessage);
    }
  },
};
