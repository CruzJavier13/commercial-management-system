export interface ApiResponse<T> {
  success: boolean;
  message?: string;
  data: T;        
  error?: string;
  details?: string;
}