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
    public course: courseDto;
    public apiUrl = 'https://localhost:44311/api/services/app/Course';

    public onSave: EventEmitter<void> = new EventEmitter<void>();
    courses: courseDto[] = [];

    constructor(
        private http: HttpClient,
        private fb: FormBuilder,
        public bsModalRef: BsModalRef
    ) { }

    ngOnInit(): void {
        this.courseForm = this.fb.group({
            id: [0],
            title: ['', Validators.required],
            description: [''],
        });
            this.bindCourseData();
    }

    private bindCourseData(): void {
        this.courseForm.setValue({
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
                                abp.notify.success('Course Updated Successfully!');
                                this.onSave.emit();
                                this.bsModalRef.hide();
                            },
                            error: (err) => {
                                abp.notify.error('Update Failed');
                                console.error('Error updating course', err);
                            }
                        });
        }
    }

    
}
