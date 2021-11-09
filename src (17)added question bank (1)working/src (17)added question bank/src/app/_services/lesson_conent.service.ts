import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';  
import { HttpHeaders } from '@angular/common/http';  
import { Observable } from 'rxjs';

import {Lesson_Content} from '../_models';

@Injectable({
  providedIn: 'root'
})
export class Lesson_ContentService {

   //Joyous, please put the link of the API here
   url = 'https://localhost:44319/api/LessonContent';  
  //  userId: any = localStorage.getItem('user');
  movies:any = localStorage.getItem("user");
  moviesi:any     = JSON.parse(this.movies);
  userId = this.moviesi['id'];
  constructor(private http: HttpClient) { }  

  getAllLesson_Content(): Observable<Lesson_Content[]> {  
    return this.http.get<Lesson_Content[]>(`${this.url}/`);  
  }  

  getLesson_ContentById(id: string): Observable<Lesson_Content> {  
      return this.http.get<Lesson_Content>(`${this.url + '/GetLesson_ContentById/' + id}`);  
    }  

  getLesson_ContentByLessonoutcomeId(id: number): Observable<Lesson_Content> {  
    return this.http.get<Lesson_Content>(`${this.url}/GetLessonContentByLessonOutcome/`+id);  
  }
  getLesson_ContentByArchieveId(id: number): Observable<Lesson_Content> {  
    return this.http.get<Lesson_Content>(`${this.url}/GetLessonContentByArchiveId/`+id);  
  } 
  delete(id: number) {
    return this.http.delete(`${this.url}/DeleteLessonContent/` + id + '/' + this.userId);
  }
  update(id:number, lesson_content: Lesson_Content) {
    return this.http.put(`${this.url}/UpdateLessonContent`+ id + '/' + this.userId, lesson_content);
  }
  create(lesson_content: Lesson_Content) {
    return this.http.post(`${this.url}/UploadContentLink/` + this.userId, lesson_content);
  }


  AchiveLessonContent(id: number, lessoncontent:Lesson_Content) {
    return this.http.put(`${this.url}/AchiveLessonContent/`+id,lessoncontent);
  }

} 