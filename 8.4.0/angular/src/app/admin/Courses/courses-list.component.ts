import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { courseDto } from '@shared/Dtos/courseDto';
import { CreateCourseComponent } from './create-course/create-course.component';
import { EditCourseComponent } from './edit-course/edit-course.component';

@Component({
    selector: 'app-courses-list',
    templateUrl: './courses-list.component.html',
})
export class CoursesListComponent implements OnInit {
    public apiUrl = 'https://localhost:44311/api/services/app/Course';
    courses: courseDto[] = [];
    currentPagedCourses: courseDto[] = [];
    searchKeyword = '';

    pageNumber = 1;
    pageSize = 5;
    totalItems = 0;

    constructor(
        private http: HttpClient,
        public bsModalRef: BsModalRef,
        private modalService: BsModalService
    ) { }

    ngOnInit(): void {
        this.loadCourseList();
    }

    public loadCourseList() {
        this.http.get<any>(`${this.apiUrl}/GetAllCourses`).subscribe({
            next: (response) => {
                debugger;
                this.courses = response.result || [];
                this.totalItems = this.courses.length;
            },
            error: (error) => {
                console.error('Error loading courses!', error);
            },
        });
    }



    public openCreateJobDialog() {
        const modalRef = this.modalService.show(CreateCourseComponent);
        modalRef.content.onSave?.subscribe(() => this.loadCourseList());
    }

    public EditCourse(course: courseDto) {
        const initialState = { course };
        const modalRef = this.modalService.show(EditCourseComponent, { initialState });
        modalRef.content.onSave?.subscribe(() => this.loadCourseList());
    }

    public DeleteCourse(course: courseDto): void {
        abp.message.confirm(
            'Are you sure you want to delete "' + course.title + '"?',
            'Delete Confirmation'
        ).then((result: boolean) => {
            if (result) {
                this.http.delete(`${this.apiUrl}/DeleteCourse?id=${course.id}`).subscribe({
                    next: () => {
                        abp.notify.success('Course deleted successfully!');
                        this.loadCourseList();
                    },
                    error: (error) => {
                        abp.notify.error('Error deleting course!');
                        console.error('Error deleting course!', error);
                    },
                });
            }
        });
    }


    public onSearch(keyword: string) {
        debugger;
        if (!keyword || keyword.trim() === '') {
            this.loadCourseList();
            return;
        }

        this.http.get<any>(`${this.apiUrl}/GetCoursesByKeyword?keyword=${keyword}`).subscribe({
            next: (response) => {
                debugger;
                this.courses = response.result || [];
                this.totalItems = this.courses.length;
            },
            error: (error) => {
                console.error('Error searching courses!', error);
            },
        });
    }



    public refresh() {
        this.searchKeyword = '';
        this.pageNumber = 1;
        this.loadCourseList();
    }
}
