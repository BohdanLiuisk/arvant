export interface BackendResult<T = null> {
  succeeded: boolean;
  data?: T;
  errors: string[];
  isFailure: boolean;
}
