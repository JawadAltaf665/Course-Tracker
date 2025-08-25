import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { enrollmentDto } from '@shared/Dtos/enrollmentDto';
import { courseDto } from '../../../../shared/Dtos/courseDto';
import { learnerDto } from '../../../../shared/Dtos/learnerDto';

@Component({
    selector: 'app-edit-enrollment',
    templateUrl: './edit-enrollment.component.html',
    //  styleUrls: ['./edit-enrollment.component.css']
})
export class EditEnrollmentComponent implements OnInit {
    public apiUrl = 'https://localhost:44311/api/services/app/Enrollment';
    public learnerApiUrl = 'https://localhost:44311/api/services/app/Learner';
    public courseApiUrl = 'https://localhost:44311/api/services/app/Course';
    public enrollmentForm: FormGroup;
    public enrollments: enrollmentDto[] = [];
    public learners: learnerDto[] = [];
    public courses: courseDto[] = [];

    public onSave: EventEmitter<void> = new EventEmitter<void>();
    enrollment: enrollmentDto;

    constructor(
        private http: HttpClient,
        private fb: FormBuilder,
        public bsModalRef: BsModalRef
    ) { }

    ngOnInit(): void {
        this.enrollmentForm = this.fb.group({
            id: 0,
            learnerId: [null, [Validators.required]],
            courseId: [null, [Validators.required]],
            completionPercentage: [0],
            isCompleted: [false],
        })
        this.loadEnrollmentList();
        this.bindModelData();
    }

    public bindModelData(): void {
        this.enrollmentForm.setValue({
            id: this.enrollment.id,
            learnerId: this.enrollment.learnerId,
            courseId: this.enrollment.courseId,
            completionPercentage: this.enrollment.completionPercentage || 0,
            isCompleted: this.enrollment.isCompleted || false,
            learnerName: this.enrollment.learnerName || '',
            courseTitle: this.enrollment.courseTitle || ''
        })
    }
    public loadEnrollmentList() {
        this.http.get<enrollmentDto>(`${this.apiUrl}/GetAllEnrollments`).subscribe({
            next: (response: any) => {
                this.enrollments = response.result || [];

                this.http.get<learnerDto>(`${this.learnerApiUrl}/GetAllLearners`).subscribe({
                    next: (response: any) => this.learners = response.result || [],
                    error: (err) => console.error('Error loading learners!', err)
                });

                this.http.get<courseDto>(`${this.courseApiUrl}/GetAllCourses`).subscribe({
                    next: (response: any) => this.courses = response.result || [],
                    error: (err) => console.error('Error loading courses!', err)
                });
            },
            error: (error) => console.error('Error loading enrollments!', error)
        });
    }
    get f() {
        return this.enrollmentForm.controls;
    }

    saveEnrollment() {
        debugger;
        if (this.enrollmentForm.invalid) {
            return;
        }

        var formData = this.enrollmentForm.value;
        console.log(formData);

        this.http.put(`${this.apiUrl}/UpdateEnrollment`, formData)
            .subscribe({
                next: (response: any) => {
                    console.log('Enrollment updated successfully!', response);
                    abp.notify.success('Enrollment updated successfully!');
                    this.onSave.emit();

                    this.bsModalRef.hide();
                },
                error: (error: any) => {
                    console.error('There was an error updating the enrollment!', error);
                    abp.notify.error('There was an error updating the enrollment!');
                }
            })
    }

    Reset() {
        this.enrollmentForm.reset();
    }
}
