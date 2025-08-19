import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, RequiredValidator, Validators } from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Observable } from 'rxjs';
import { courseDto } from '@shared/Dtos/courseDto'; // Adjust the import path as necessary }


@Component({
    selector: 'app-courses-list',
    templateUrl: './courses-list.component.html',
    //  styleUrls: ['./courses-list.component.css']
})
export class CoursesListComponent implements OnInit {
    public courseForm: FormGroup;
    public apiUrl = 'https://localhost:44311/api/services/app/Course';
    courses: any[] = [];
    searchKeyword = '';
    filteredCourses: any[] = [];
    courseDto: courseDto[] = [];

    constructor(
        private http: HttpClient,
        private fb: FormBuilder,
        public bsModalRef: BsModalRef,
    ) { }

    ngOnInit(): void {
        this.courseForm = this.fb.group({
            id: 0,
            title: ['', [Validators.required]],
            description: [''],
        })
        this.loadCourseList();
    }

    get f() {
        return this.courseForm.controls;
    }

    public loadCourseList() {
        debugger;
        this.http.get<courseDto[]>(`${this.apiUrl}/GetAllCourses`)
            .subscribe({
                next: (response: any) => {
                    debugger;
                    this.courses = response.result;
                },
                error: (error: any) => {
                    console.error('There was an error!', error);
                }
            })
    }

    public Save() {
        debugger;
        if (this.courseForm.invalid) {
            return;
        }
        var formData = this.courseForm.value;
        console.log(formData);

        if (formData.id == 0) {
            this.http.post(`${this.apiUrl}/CreateCourse`, formData)
                .subscribe({
                    next: (response: any) => {
                        console.log('Course created successfully!', response);
                        this.courseForm.reset();
                        this.loadCourseList();
                    },
                    error: (error: any) => {
                        console.error('There was an error!', error);
                    }
                })
        }
        else {
            this.http.put(`${this.apiUrl}/UpdateCourse`, formData)
                .subscribe({
                    next: (response: any) => {
                        console.log('Course updated successfully!', response);
                        this.courseForm.reset();
                        this.loadCourseList();
                    },
                    error: (error: any) => {
                        console.error('There was an error!', error);
                    }
                });

        }
    }

    public Reset() {
        this.courseForm.reset();
    }

    public EditCourse(course: any) {
        debugger;
        this.courseForm.patchValue({
            id: course.id,
            title: course.title,
            description: course.description
        });
    }

    public DeleteCourse(course: any) {
        debugger;
        if (confirm(`Are you sure you want to delete ${course.title}?`)) {
            this.http.delete(`${this.apiUrl}/DeleteCourse?id=${course.id}`)
                .subscribe({
                    next: (response: any) => {
                        console.log('Course deleted successfully!', response);
                        this.loadCourseList();
                    },
                    error: (error: any) => {
                        console.error('There was an error!', error);
                    }
                });
        }
    }

    public onSearch(searchKeyword: string) {
        debugger;
        this.http.get<any>(`${this.apiUrl}/GetCoursesByKeyword?keyword=${searchKeyword}`)
            .subscribe({
                next: (response: any) => {
                    this.filteredCourses = response.result;
                },
                error: (error: any) => {
                    console.error('There was an error!', error);
                }
            })
    }
}
