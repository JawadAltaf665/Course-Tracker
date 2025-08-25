import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-create-enrollment',
  templateUrl: './create-enrollment.component.html',
})
export class CreateEnrollmentComponent implements OnInit {
    public apiUrl = 'https://localhost:44311/api/services/app/Enrollment';
    public learnerApiUrl = 'https://localhost:44311/api/services/app/Learner';
    public courseApiUrl = 'https://localhost:44311/api/services/app/Course';
    public enrollmentForm: FormGroup;
    public enrollments: any[] = [];
    public learners: any[] = [];
    public courses: any[] = [];

    public onSave: EventEmitter<void> = new EventEmitter<void>();

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
    }

    public loadEnrollmentList() {
        this.http.get<any>(`${this.apiUrl}/GetAllEnrollments`).subscribe({
            next: (response) => {
                this.enrollments = response.result || [];

                // Load learners
                this.http.get<any>(`${this.learnerApiUrl}/GetAllLearners`).subscribe({
                    next: (res) => this.learners = res.result || [],
                    error: (err) => console.error('Error loading learners!', err)
                });

                // Load courses
                this.http.get<any>(`${this.courseApiUrl}/GetAllCourses`).subscribe({
                    next: (res) => this.courses = res.result || [],
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
        //debugger;
        if (this.enrollmentForm.invalid) {
            return;
        }

        var formData = this.enrollmentForm.value;
        console.log(formData);

            this.http.post(`${this.apiUrl}/CreateEnrollment`, formData)
                .subscribe({
                    next: (response: any) => {
                        console.log('Enrollment created successfully!', response);
                        abp.notify.success('Enrollment created successfully!');
                        this.onSave.emit();

                        this.bsModalRef.hide();
                    },
                    error: (error: any) => {
                        console.error('There was an error creating the enrollment!', error);
                        abp.notify.error('There was an error creating the enrollment!');
                    }
                })
    }

    Reset() {
        this.enrollmentForm.reset();
    }
}
