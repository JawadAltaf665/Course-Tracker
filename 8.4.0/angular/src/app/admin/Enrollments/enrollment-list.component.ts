import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-enrollment-list',
  templateUrl: './enrollment-list.component.html',
//  styleUrls: ['./enrollment-list.component.css']
})
export class EnrollmentListComponent implements OnInit {
    public apiUrl = 'https://localhost:44311/api/services/app/Enrollment';
    public learnerApiUrl = 'https://localhost:44311/api/services/app/Learner';
    public courseApiUrl = 'https://localhost:44311/api/services/app/Course';
    public enrollmentForm: FormGroup;
    public enrollments: any[] = [];
    public learners: any[] = [];
    public courses: any[] = [];

    constructor(
        private fb: FormBuilder,
        private http: HttpClient
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
        debugger;
        this.http.get<any[]>(`${this.apiUrl}/GetAllEnrollments`)
            .subscribe({
                next: (response: any) => {
                    this.enrollments = response.result;
                    console.warn('Enrollments loaded successfully:', this.enrollments);

                    this.http.get<any[]>(`${this.learnerApiUrl}/GetAllLearners`)
                        .subscribe({
                            next: (learnerResponse: any) => {
                                this.learners = learnerResponse.result;
                                console.warn('Learners loaded successfully:', this.learners);
                            },
                            error: (err: any) => {
                                console.error('Error loading learners!', err);
                            }
                        })

                    this.http.get<any[]>(`${this.courseApiUrl}/GetAllCourses`)
                        .subscribe({
                            next: (courseResponse: any) => {
                                this.courses = courseResponse.result;
                                console.warn('Courses loaded successfully:', this.courses);
                            },
                            error: (err: any) => {
                                console.error('Error loading courses!', err);
                            }
                        })
                },
                error: (error: any) => {
                    console.error('There was an error loading enrollments!', error);
                }
            })
    }

    Save() {
        debugger;
        if( this.enrollmentForm.invalid) {
            return;
        }

        var formData = this.enrollmentForm.value;
        console.log(formData);

        if(formData.id == 0) {
            this.http.post(`${this.apiUrl}/CreateEnrollment`, formData)
                .subscribe({
                    next: (response: any) => {
                        console.log('Enrollment created successfully!', response);
                        this.enrollmentForm.reset();
                        this.loadEnrollmentList();
                    },
                    error: (error: any) => {
                        console.error('There was an error creating the enrollment!', error);
                    }
                })
        } else {
            this.http.put(`${this.apiUrl}/UpdateEnrollment`, formData)
                .subscribe({
                    next: (response: any) => {
                        console.log('Enrollment updated successfully!', response);
                        this.enrollmentForm.reset();
                        this.loadEnrollmentList();
                    },
                    error: (error: any) => {
                        console.error('There was an error updating the enrollment!', error);
                    }
                })
        }
    }

    Reset() {
        this.enrollmentForm.reset();
    }

    EditEnrollment(enrollment: any) {
        debugger;
        this.enrollmentForm.patchValue({
            id: enrollment.id,
            learnerName: enrollment.learnerName,
            courseTitle: enrollment.courseTitle,
            learnerId: enrollment.learnerId,
            courseId: enrollment.courseId,
            completionPercentage: enrollment.completionPercentage,
            isCompleted: enrollment.isCompleted
        })
    }

    DeleteEnrollment(enrollment: any) {
        debugger;
        if (confirm(`Are you sure you want to delete the enrollment for ${enrollment.learnerName} in ${enrollment.courseName}?`)) {
            this.http.delete(`${this.apiUrl}/DeleteEnrollment?id=${enrollment.id}`)
                .subscribe({
                    next: (response: any) => {
                        console.log('Enrollment deleted successfully!', response);
                        this.loadEnrollmentList();
                    },
                    error: (error: any) => {
                        console.error('There was an error deleting the enrollment!', error);
                    }
                })
        }
    }
}
