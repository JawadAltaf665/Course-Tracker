import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
    selector: 'app-create-course',
    templateUrl: './create-course.component.html',
})
export class CreateCourseComponent implements OnInit {
    public courseForm: FormGroup;
    public apiUrl = 'https://localhost:44311/api/services/app/Course';
    saving = false;

    public onSave: EventEmitter<void> = new EventEmitter<void>();

    constructor(
        private http: HttpClient,
        private fb: FormBuilder,
        public bsModalRef: BsModalRef
    ) { }

    ngOnInit(): void {
        this.courseForm = this.fb.group({
            id: [0],
            title: ['', [Validators.required]],
            description: [''],
        });
    }

    get f() {
        return this.courseForm.controls;
    }

    public saveCourse() {   
        if (this.courseForm.invalid) {
            return;
        }
        this.saving = true;
        const formData = this.courseForm.value;

        if (formData.id === 0) {
            this.http.post(`${this.apiUrl}/CreateCourse`, formData).subscribe({
                next: (response: any) => {
                    console.log('Course created successfully!', response);
                    this.saving = false;

                    this.onSave.emit();

                    this.bsModalRef.hide();
                },
                error: (error: any) => {
                    console.error('There was an error!', error);
                    this.saving = false;
                },
            });
        } else {
            // 🔹 Update course
            this.http.put(`${this.apiUrl}/UpdateCourse`, formData).subscribe({
                next: (response: any) => {
                    console.log('Course updated successfully!', response);
                    this.saving = false;

                    // 🔹 Emit event before closing
                    this.onSave.emit();

                    this.bsModalRef.hide();
                },
                error: (error: any) => {
                    console.error('There was an error!', error);
                    this.saving = false;
                },
            });
        }
    }

    public Reset() {
        this.courseForm.reset({
            id: 0,
            title: '',
            description: '',
        });
    }
}
