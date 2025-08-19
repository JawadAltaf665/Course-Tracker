import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { courseDto } from '@shared/Dtos/courseDto';


@Component({
    selector: 'app-module-list',
    templateUrl: './module-list.component.html',
    //  styleUrls: ['./module-list.component.css']
})
export class ModuleListComponent implements OnInit {
    public moduleForm: FormGroup;
    public apiUrl = 'https://localhost:44311/api/services/app/Module';
    public courseApiUrl = 'https://localhost:44311/api/services/app/Course';
    public courses: any[] = [];
    public modules: any[] = [];

    constructor(
        private fb: FormBuilder,
        private http: HttpClient,
    ) { }

    ngOnInit(): void {
        this.moduleForm = this.fb.group({
            id: 0,
            title: ['', [Validators.required, Validators.minLength(3)]],
            description: [''],
            courseId: [null, Validators.required],
        })
        this.loadModuleList();
    }

    get f() {
        return this.moduleForm.controls;
    }

    public loadModuleList() {

        this.http.get<any[]>(`${this.apiUrl}/GetAllModules`).subscribe({
            next: (moduleRes: any) => {
                debugger;
                this.modules = moduleRes.result;

                this.http.get(`${this.courseApiUrl}/GetAllCourses`).subscribe({
                    next: (courseRes: any) => {
                        this.courses = courseRes.result;
                    },
                    error: (err: any) => console.error('Error loading courses!', err)
                });
            },
            error: (err: any) => console.error('Error loading modules!', err)
        });
    }


    public Save() {
        debugger;
        if (this.moduleForm.invalid) {
            return;
        }
        var formData = this.moduleForm.value;
        console.log(formData);

        if (formData.id == 0) {
            this.http.post(`${this.apiUrl}/CreateModule`, formData)
                .subscribe({
                    next: (response: any) => {
                        console.log('Module created successfully!', response);
                        this.loadModuleList();
                        this.moduleForm.reset();
                    },
                    error: (error: any) => {
                        console.error('There was an error!', error);
                    }
                })
        } else {
            this.http.put(`${this.apiUrl}/UpdateModule`, formData)
                .subscribe({
                    next: (response: any) => {
                        console.log('Module updated successfully!', response);
                        this.loadModuleList();
                        this.moduleForm.reset();
                    },
                    error: (error: any) => {
                        console.error('There was an error!', error);
                    }
                })
        }
        
    }

    Reset() {
        this.moduleForm.reset();
    }

    DeleteModule(module: any) {
        debugger;
        if (confirm(`Are you sure you want to delete ${module.title}?`)) {
            this.http.delete(`${this.apiUrl}/DeleteModule?id=${module.id}`)
                .subscribe({
                    next: (response: any) => {
                        console.warn('Module deleted successfully!', response);
                        this.loadModuleList();
                    },
                    error: (error: any) => {
                        console.error('There was an error deleting the module!', error);
                    }
                });
        }
    }

    EditModule(module: any) {
        debugger;
        this.moduleForm.patchValue({
            id: module.id,
            title: module.title,
            description: module.description,
            courseId: module.courseId
        })
    }
}
