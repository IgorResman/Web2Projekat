import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';


@Injectable()
export class AuthHttpService {
    base_url = "http://localhost:52295"
    user: string
    constructor(private http: HttpClient) { }
}