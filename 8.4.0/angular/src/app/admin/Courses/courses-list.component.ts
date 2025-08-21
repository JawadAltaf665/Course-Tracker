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
    courses: courseDto[] = [];          // Full list from API
    pagedCourses: courseDto[] = [];     // Current page data (for pagination)
    searchKeyword = '';

    // 🔹 Pagination props
    pageNumber = 1;
    pageSize = 5;
    totalItems = 0;

    // API Base URL
    public apiUrl = 'https://localhost:44311/api/services/app/Course';

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
                this.courses = response.result || [];
                this.totalItems = this.courses.length;
                // 👇 The fix: Call getDataPage() to populate pagedCourses
                this.getDataPage(this.pageNumber);
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

    public DeleteCourse(course: courseDto) {
        if (confirm(`Are you sure you want to delete ${course.title}?`)) {
            this.http.delete(`${this.apiUrl}/DeleteCourse?id=${course.id}`).subscribe({
                next: () => {
                    console.log('Course deleted successfully!');
                    this.loadCourseList();
                },
                error: (error) => {
                    console.error('Error deleting course!', error);
                },
            });
        }
    }

    public onSearch(keyword: string) {
        if (!keyword || keyword.trim() === '') {
            this.loadCourseList();
            return;
        }

        this.http.get<any>(`${this.apiUrl}/GetCoursesByKeyword?keyword=${keyword}`).subscribe({
            next: (response) => {
                debugger;
                this.courses = response.result || [];
                this.totalItems = this.courses.length;
                this.getDataPage(this.pageNumber); // Apply pagination
            },
            error: (error) => {
                console.error('Error searching courses!', error);
            },
        });
    }

  

    public getDataPage(page: number) {
        this.pageNumber = page;
        const startIndex = (page - 1) * this.pageSize;
        const endIndex = startIndex + this.pageSize;
        this.pagedCourses = this.courses.slice(startIndex, endIndex);
    }

    public refresh() {
        this.searchKeyword = '';
        this.pageNumber = 1; // Reset to the first page on refresh
        this.loadCourseList();
    }
}
