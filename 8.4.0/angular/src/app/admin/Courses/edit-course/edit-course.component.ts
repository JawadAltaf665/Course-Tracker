import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { courseDto } from '../../../../shared/Dtos/courseDto';

@Component({
    selector: 'app-edit-course',
    templateUrl: './edit-course.component.html',
//    styleUrls: ['./edit-course.component.css']
})
export class EditCourseComponent implements OnInit {
    public courseForm: FormGroup;
    public course!: courseDto;
    public apiUrl = 'https://localhost:44311/api/services/app/Course';

    // 🔹 Add this emitter
    public onSave: EventEmitter<void> = new EventEmitter<void>();

    constructor(
        private http: HttpClient,
        private fb: FormBuilder,
        public bsModalRef: BsModalRef
    ) {
        this.courseForm = this.fb.group({
            id: [0],
            title: ['', Validators.required],
            description: ['']
        });
    }

    ngOnInit(): void {
        if (this.course) {
            this.bindCourseData();
        }
    }

    private bindCourseData(): void {
        this.courseForm.patchValue({
            id: this.course.id,
            title: this.course.title,
            description: this.course.description || ''
        });
    }

    public saveCourse(): void { 
        if (this.courseForm.valid) {
            const formData = this.courseForm.value;

            this.http.put(`${this.apiUrl}/UpdateCourse`, formData).subscribe({
                next: (res) => {
                    console.log('Course updated successfully', res);
                    this.onSave.emit();

                    this.bsModalRef.hide();
                },
                error: (err) => {
                    console.error('Error updating course', err);
                }
            });
        } else {
            this.markFormGroupTouched();
        }
    }

    private markFormGroupTouched(): void {
        Object.keys(this.courseForm.controls).forEach(key => {
            const control = this.courseForm.get(key);
            control?.markAsTouched();
        });
    }
}
