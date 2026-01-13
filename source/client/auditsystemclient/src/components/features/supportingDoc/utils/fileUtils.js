export function getFileType(fileName) { 
  const extension = fileName.substring(fileName.lastIndexOf('.') + 1).toLowerCase(); 
  const fileTypes = { 
    'pdf': 'PDF Document', 
    'doc': 'Word Document', 
    'docx': 'Word Document', 
    'xlsx': 'Excel Spreadsheet', 
    'ppt': 'PowerPoint Presentation',
    'pptx': 'PowerPoint Presentation',
    'jpg': 'Image', 
    'jpeg': 'Image', 
    'png': 'Image', 
    'gif': 'Image', 
    'mp4': 'Video', 
    'mkv': 'Video'
  }; 
  return fileTypes[extension] || 'Unknown'; 
}

export function formatFileSize(bytes) {
  if (bytes === 0) return '0 Bytes';
  const k = 1024;
  const sizes = ['Bytes', 'KB', 'MB', 'GB'];
  const i = Math.floor(Math.log(bytes) / Math.log(k));
  return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
}

export function validateFileType(file, allowedTypes = []) {
  if (allowedTypes.length === 0) return true;
  const fileExtension = file.name.substring(file.name.lastIndexOf('.') + 1).toLowerCase();
  return allowedTypes.includes(fileExtension);
}

export function validateFileSize(file, maxSizeInMB = 10) {
  const maxSizeInBytes = maxSizeInMB * 1024 * 1024;
  return file.size <= maxSizeInBytes;
}

/**
 * Validates file name for security issues
 * @param {string} fileName - The file name to validate
 * @returns {object} - { isValid: boolean, error: string|null }
 */
export function validateFileName(fileName) {
  if (!fileName || fileName.trim() === '') {
    return { isValid: false, error: 'File name cannot be empty' };
  }

  // Check for path traversal attempts
  if (fileName.includes('..') || fileName.includes('/') || fileName.includes('\\')) {
    return { isValid: false, error: 'File name contains invalid path characters' };
  }

  // Check for dangerous characters
  const dangerousChars = /[<>:"|?*\x00-\x1f]/;
  if (dangerousChars.test(fileName)) {
    return { isValid: false, error: 'File name contains invalid characters' };
  }

  // Check length
  if (fileName.length > 255) {
    return { isValid: false, error: 'File name is too long (max 255 characters)' };
  }

  return { isValid: true, error: null };
}