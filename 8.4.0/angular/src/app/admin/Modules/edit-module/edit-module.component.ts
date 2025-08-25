import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { courseDto } from '../../../../shared/Dtos/courseDto';
import { moduleDto } from '../../../../shared/Dtos/moduleDto';

@Component({
    selector: 'app-edit-module',
    templateUrl: './edit-module.component.html',
    //  styleUrls: ['./edit-module.component.css']
})
export class EditModuleComponent implements OnInit {
    public moduleForm: FormGroup;
    public module: moduleDto;
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
        this.bindModuleData();
        this.loadModuleList();
    }

    get f() {
        return this.moduleForm.controls;
    }

    private bindModuleData(): void {
        this.moduleForm.setValue({
            id: this.module.id,
            title: this.module.title,
            description: this.module.description || '',
            courseId: this.module.courseId
        });
    }

    public loadModuleList() {

        this.http.get<moduleDto[]>(`${this.apiUrl}/GetAllModules`).subscribe({
            next: (response: any) => {
                this.modules = response.result;

                this.http.get<courseDto[]>(`${this.courseApiUrl}/GetAllCourses`).subscribe({
                    next: (response: any) => {
                        this.courses = response.result;
                    },
                    error: (err: any) => console.error('Error loading courses!', err)
                });
            },
            error: (err: any) => console.error('Error loading modules!', err)
        });
    }

    public saveModule() {
        //debugger;
        if (this.moduleForm.invalid) {
            return;
        }
        var formData = this.moduleForm.value;
        console.log(formData);

        this.http.put(`${this.apiUrl}/UpdateModule`, formData)
            .subscribe({
                next: (response: any) => {
                    console.log('Module updated successfully!', response);
                    abp.notify.success('Module updated successfully!');
                    this.onSave.emit();

                    this.bsModalRef.hide();
                },
                error: (error: any) => {
                    console.error('There was an error!', error);
                    abp.notify.error('There was an error updating the module!');
                }
            })

    }

    Reset() {
        this.moduleForm.reset();
    }

}
