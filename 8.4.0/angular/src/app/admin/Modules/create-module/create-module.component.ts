import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { courseDto } from '../../../../shared/Dtos/courseDto';
import { moduleDto } from '../../../../shared/Dtos/moduleDto';

@Component({
    selector: 'app-create-module',
    templateUrl: './create-module.component.html',
    //  styleUrls: ['./create-module.component.css']
})
export class CreateModuleComponent {
    public moduleForm: FormGroup;
    public apiUrl = 'https://localhost:44311/api/services/app/Module';
    public courseApiUrl = 'https://localhost:44311/api/services/app/Course';
    public courses: courseDto[] = [];
    public modules: moduleDto[] = [];

    public onSave: EventEmitter<void> = new EventEmitter<void>();

    constructor(
        private http: HttpClient,
        private fb: FormBuilder,
        public bsModalRef: BsModalRef
    ) { }

    ngOnInit(): void {
        this.moduleForm = this.fb.group({
            id: 0,
            title: ['', [Validators.required, Validators.minLength(3)]],
            description: [''],
            courseId: [null, Validators.required],
        })
        this.loadCourseList();
    }

    get f() {
        return this.moduleForm.controls;
    }

    public loadCourseList() {
        this.http.get<courseDto>(`${this.courseApiUrl}/GetAllCourses`).subscribe({
            next: (response: any) => {
                this.courses = response.result;
                console.warn('Courses loaded successfully:', this.courses);
            },
            error: (err: any) => console.error('Error loading courses!', err)
        });
    }


    public saveModule() {
        //debugger;
        if (this.moduleForm.invalid) {
            return;
        }
        var formData = this.moduleForm.value;
        console.log(formData);

        this.http.post(`${this.apiUrl}/CreateModule`, formData)
            .subscribe({
                next: (response: any) => {
                    console.log('Module created successfully!', response);
                    abp.notify.success('Module created successfully!');
                    this.onSave.emit();

                    this.bsModalRef.hide();
                },
                error: (error: any) => {
                    console.error('There was an error!', error);
                    abp.notify.error('There was an error creating the module!');
                }
            })
    }

    Reset() {
        this.moduleForm.reset();
    }
}
