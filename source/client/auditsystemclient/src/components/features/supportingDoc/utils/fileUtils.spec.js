import { describe, it, expect } from 'vitest';
import { 
  validateFileType, 
  validateFileSize, 
  formatFileSize,
  validateFileName,
  getFileType 
} from './fileUtils.js';

describe('fileUtils - validateFileType', () => {
  it('should accept files with allowed extensions', () => {
    const allowedTypes = ['pdf', 'docx', 'xlsx'];
    
    const pdfFile = new File(['content'], 'document.pdf', { type: 'application/pdf' });
    const docxFile = new File(['content'], 'document.docx', { type: 'application/vnd.openxmlformats-officedocument.wordprocessingml.document' });
    
    expect(validateFileType(pdfFile, allowedTypes)).toBe(true);
    expect(validateFileType(docxFile, allowedTypes)).toBe(true);
  });

  it('should reject files with disallowed extensions', () => {
    const allowedTypes = ['pdf', 'docx'];
    
    const exeFile = new File(['content'], 'malicious.exe', { type: 'application/x-msdownload' });
    const jsFile = new File(['content'], 'script.js', { type: 'text/javascript' });
    
    expect(validateFileType(exeFile, allowedTypes)).toBe(false);
    expect(validateFileType(jsFile, allowedTypes)).toBe(false);
  });

  it('should handle case-insensitive extension matching', () => {
    const allowedTypes = ['pdf', 'docx'];
    
    const upperCaseFile = new File(['content'], 'document.PDF', { type: 'application/pdf' });
    const mixedCaseFile = new File(['content'], 'document.DocX', { type: 'application/vnd.openxmlformats-officedocument.wordprocessingml.document' });
    
    expect(validateFileType(upperCaseFile, allowedTypes)).toBe(true);
    expect(validateFileType(mixedCaseFile, allowedTypes)).toBe(true);
  });

  it('should reject files with double extensions', () => {
    const allowedTypes = ['pdf', 'docx'];
    
    const doubleExtFile = new File(['content'], 'document.pdf.exe', { type: 'application/x-msdownload' });
    
    expect(validateFileType(doubleExtFile, allowedTypes)).toBe(false);
  });

  it('should reject files without extensions', () => {
    const allowedTypes = ['pdf', 'docx'];
    
    const noExtFile = new File(['content'], 'documentwithoutextension', { type: 'application/octet-stream' });
    
    expect(validateFileType(noExtFile, allowedTypes)).toBe(false);
  });

  it('should return true when no allowed types are specified', () => {
    const anyFile = new File(['content'], 'document.pdf', { type: 'application/pdf' });
    
    expect(validateFileType(anyFile, [])).toBe(true);
  });
});

describe('fileUtils - validateFileSize', () => {
  it('should accept files within size limit', () => {
    const smallFile = new File(['a'.repeat(1024 * 1024)], 'small.pdf'); // 1MB
    
    expect(validateFileSize(smallFile, 10)).toBe(true);
  });

  it('should accept files exactly at size limit', () => {
    const exactFile = new File(['a'.repeat(10 * 1024 * 1024)], 'exact.pdf'); // 10MB
    
    expect(validateFileSize(exactFile, 10)).toBe(true);
  });

  it('should reject files over size limit', () => {
    const largeFile = new File(['a'.repeat(11 * 1024 * 1024)], 'large.pdf'); // 11MB
    
    expect(validateFileSize(largeFile, 10)).toBe(false);
  });

  it('should handle custom size limits', () => {
    const file = new File(['a'.repeat(50 * 1024 * 1024)], 'file.pdf'); // 50MB
    
    expect(validateFileSize(file, 100)).toBe(true);
    expect(validateFileSize(file, 25)).toBe(false);
  });
});

describe('fileUtils - formatFileSize', () => {
  it('should format bytes correctly', () => {
    expect(formatFileSize(0)).toBe('0 Bytes');
    expect(formatFileSize(1024)).toBe('1 KB');
    expect(formatFileSize(1024 * 1024)).toBe('1 MB');
    expect(formatFileSize(1024 * 1024 * 1024)).toBe('1 GB');
  });

  it('should format decimal values correctly', () => {
    expect(formatFileSize(1536)).toBe('1.5 KB'); // 1.5 KB
    expect(formatFileSize(2.5 * 1024 * 1024)).toBe('2.5 MB'); // 2.5 MB
  });
});

describe('fileUtils - validateFileName', () => {
  it('should accept valid file names', () => {
    const result = validateFileName('document.pdf');
    
    expect(result.isValid).toBe(true);
    expect(result.error).toBeNull();
  });

  it('should reject empty file names', () => {
    const result = validateFileName('');
    
    expect(result.isValid).toBe(false);
    expect(result.error).toContain('empty');
  });

  it('should reject file names with path traversal attempts', () => {
    const pathTraversalTests = [
      '../../../etc/passwd',
      '..\\..\\windows\\system32',
      'folder/../file.pdf',
      'folder\\file.pdf'
    ];

    pathTraversalTests.forEach(fileName => {
      const result = validateFileName(fileName);
      expect(result.isValid).toBe(false);
      expect(result.error).toContain('invalid path characters');
    });
  });

  it('should reject file names with dangerous characters', () => {
    const dangerousNames = [
      'file<script>.pdf',
      'file>output.pdf',
      'file:name.pdf',
      'file"name.pdf',
      'file|name.pdf',
      'file?name.pdf',
      'file*name.pdf'
    ];

    dangerousNames.forEach(fileName => {
      const result = validateFileName(fileName);
      expect(result.isValid).toBe(false);
      expect(result.error).toContain('invalid characters');
    });
  });

  it('should reject file names that are too long', () => {
    const longFileName = 'a'.repeat(256) + '.pdf';
    const result = validateFileName(longFileName);
    
    expect(result.isValid).toBe(false);
    expect(result.error).toContain('too long');
  });

  it('should accept file names at maximum length', () => {
    const maxLengthFileName = 'a'.repeat(251) + '.pdf'; // 255 total
    const result = validateFileName(maxLengthFileName);
    
    expect(result.isValid).toBe(true);
  });

  it('should accept file names with allowed special characters', () => {
    const validNames = [
      'file-name.pdf',
      'file_name.pdf',
      'file.name.pdf',
      'file (1).pdf',
      'file [copy].pdf'
    ];

    validNames.forEach(fileName => {
      const result = validateFileName(fileName);
      expect(result.isValid).toBe(true);
    });
  });
});

describe('fileUtils - getFileType', () => {
  it('should return correct file type descriptions', () => {
    expect(getFileType('document.pdf')).toBe('PDF Document');
    expect(getFileType('document.docx')).toBe('Word Document');
    expect(getFileType('spreadsheet.xlsx')).toBe('Excel Spreadsheet');
    expect(getFileType('presentation.pptx')).toBe('PowerPoint Presentation');
  });

  it('should handle case-insensitive extensions', () => {
    expect(getFileType('document.PDF')).toBe('PDF Document');
    expect(getFileType('document.DOCX')).toBe('Word Document');
  });

  it('should return Unknown for unrecognized extensions', () => {
    expect(getFileType('file.xyz')).toBe('Unknown');
    expect(getFileType('file.exe')).toBe('Unknown');
  });
});
