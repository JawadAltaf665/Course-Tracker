import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { courseDto } from '../../../shared/Dtos/courseDto';
import { enrollmentDto } from '../../../shared/Dtos/enrollmentDto';
import { learnerDto } from '../../../shared/Dtos/learnerDto';
import { CreateEnrollmentComponent } from './create-enrollment/create-enrollment.component';
import { EditEnrollmentComponent } from './edit-enrollment/edit-enrollment.component';

@Component({
    selector: 'app-enrollment-list',
    templateUrl: './enrollment-list.component.html',
})
export class EnrollmentListComponent implements OnInit {
    public apiUrl = 'https://localhost:44311/api/services/app/Enrollment';
    public learnerApiUrl = 'https://localhost:44311/api/services/app/Learner';
    public courseApiUrl = 'https://localhost:44311/api/services/app/Course';

    enrollments: enrollmentDto[] = [];
    learners: learnerDto[] = [];
    courses: courseDto[] = [];
    currentPagedEnrollments: enrollmentDto[] = [];

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
        this.loadEnrollmentList();
    }

    public loadEnrollmentList() {
        this.http.get<any>(`${this.apiUrl}/GetAllEnrollments`).subscribe({
            next: (response) => {
                this.enrollments = response.result || [];
                this.totalItems = this.enrollments.length;

                this.http.get<any>(`${this.learnerApiUrl}/GetAllLearners`).subscribe({
                    next: (res) => this.learners = res.result || [],
                    error: (err) => console.error('Error loading learners!', err)
                });

                this.http.get<any>(`${this.courseApiUrl}/GetAllCourses`).subscribe({
                    next: (res) => this.courses = res.result || [],
                    error: (err) => console.error('Error loading courses!', err)
                });
            },
            error: (error) => console.error('Error loading enrollments!', error)
        });
    }

    public openCreateEnrollmentDialog() {
        const modalRef = this.modalService.show(CreateEnrollmentComponent);
        modalRef.content.onSave?.subscribe(() => this.loadEnrollmentList());
    }

    public EditEnrollment(enrollment: enrollmentDto) {
        const initialState = { enrollment };
        const modalRef = this.modalService.show(EditEnrollmentComponent, { initialState });
        modalRef.content.onSave?.subscribe(() => this.loadEnrollmentList());
    }

    public DeleteEnrollment(enrollment: enrollmentDto): void {
        abp.message.confirm(
            'Are you sure you want to delete enrollment of"' + enrollment.learnerName + " in " + enrollment.courseTitle +'"?',
            'Delete Confirmation'
        ).then((result: boolean) => {
            if (result) {
                this.http.delete(`${this.apiUrl}/DeleteEnrollment?id=${enrollment.id}`).subscribe({
                    next: () => {
                        abp.notify.success('Enrollment deleted successfully!');
                        this.loadEnrollmentList();
                    },
                    error: (error) => {
                        abp.notify.error('Error deleting enrollment!');
                        console.error('Error deleting enrollment!', error);
                    }
                });
            }
        });
    }


    public onSearch(keyword: string) {
        if (!keyword || keyword.trim() === '') {
            this.loadEnrollmentList();
            return;
        }

        this.http.get<any>(`${this.apiUrl}/GetEnrollmentsByKeyword?keyword=${keyword}`).subscribe({
            next: (response) => {
                this.enrollments = response.result || [];
                this.totalItems = this.enrollments.length;
            },
            error: (error) => console.error('Error searching enrollments!', error)
        });
    }

    public refresh() {
        this.searchKeyword = '';
        this.pageNumber = 1;
        this.loadEnrollmentList();
    }
}
