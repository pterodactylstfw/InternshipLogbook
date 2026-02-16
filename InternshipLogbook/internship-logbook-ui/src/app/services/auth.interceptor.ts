import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Auth} from './auth';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(Auth);
  const token = authService.getToken();

  // Dacă avem token, îl atașăm în Header
  if (token) {
    const clonedReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
    return next(clonedReq);
  }

  // Dacă nu avem token, trimitem cererea așa cum e (ex: login)
  return next(req);
};
