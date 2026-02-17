import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Auth} from './auth';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(Auth);
  const token = authService.getToken();

  if (req.url.includes('/Auth/login'))
    return next(req); // sar peste token daca sunt la login

  if (token) {
    const clonedReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
    return next(clonedReq);
  }
  return next(req);
};
