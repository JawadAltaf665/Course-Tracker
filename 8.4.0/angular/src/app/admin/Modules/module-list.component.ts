import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { FormBuilder, FormGroup } from '@angular/forms';
import { moduleDto } from '@shared/Dtos/moduleDto';
import { CreateModuleComponent } from './create-module/create-module.component';
import { EditModuleComponent } from './edit-module/edit-module.component';
import { courseDto } from '../../../shared/Dtos/courseDto';

@Component({
    selector: 'app-module-list',
    templateUrl: './module-list.component.html',
})
export class ModuleListComponent implements OnInit {
    public moduleForm: FormGroup;
    public apiUrl = 'https://localhost:44311/api/services/app/Module';
    public courseApiUrl = 'https://localhost:44311/api/services/app/Course';

    public courses: courseDto[] = [];  
    public modules: moduleDto[] = [];   
    public currentPagedModules: moduleDto[] = []; 

    public searchKeyword = '';
    public pageNumber = 1;
    public pageSize = 5;
    public totalItems = 0;

    constructor(
        private http: HttpClient,
        public bsModalRef: BsModalRef,
        private modalService: BsModalService,
        private fb: FormBuilder
    ) {
        this.moduleForm = this.fb.group({
            title: [''],
            description: [''],
            courseId: [null],
        });
    }

    ngOnInit(): void {
        this.loadModuleList();
    }

    public loadModuleList() {
        this.http.get<moduleDto>(`${this.apiUrl}/GetAllModules`).subscribe({
            next: (response: any) => {
                this.modules = response.result;
                this.totalItems = this.modules.length;

                this.http.get<courseDto>(`${this.courseApiUrl}/GetAllCourses`).subscribe({
                    next: (response: any) => {
                        this.courses = response.result;
                    },
                    error: (err) => {
                        console.error('Error loading courses!', err);
                    },
                });
            },
            error: (err) => {
                console.error('Error loading modules!', err);
            },
        });
    }

    public CreateModule() {
        const modalRef = this.modalService.show(CreateModuleComponent);
        modalRef.content.onSave?.subscribe(() => this.loadModuleList());
    }

    public EditModule(module: moduleDto) {
        const initialState = { module };
        const modalRef = this.modalService.show(EditModuleComponent, { initialState });
        modalRef.content.onSave?.subscribe(() => this.loadModuleList());
    }

    public DeleteModule(module: moduleDto): void {
        abp.message.confirm(
            `Are you sure you want to delete "${module.title}"?`,
            'Delete Confirmation'
        ).then((result: boolean) => {
            if (result) {
                this.http.delete(`${this.apiUrl}/DeleteModule?id=${module.id}`).subscribe({
                    next: () => {
                        abp.notify.success('Module deleted successfully!');
                        this.loadModuleList();
                    },
                    error: (error) => {
                        abp.notify.error('Error deleting module!');
                        console.error('Error deleting module!', error);
                    }
                });
            }
        });
    }


    public onSearch(keyword: string) {
        if (!keyword || keyword.trim() === '') {
            this.loadModuleList();
            return;
        }

        this.http.get<any>(`${this.apiUrl}/GetModulesByKeyword?keyword=${keyword}`)
            .subscribe({
                next: (res) => {
                    this.modules = res.result || [];
                
                    this.totalItems = this.modules.length;
                },
                error: (err) => {
                    console.error('Error searching modules!', err);
                },
            });
    }

    public refresh() {
        this.searchKeyword = '';
        this.pageNumber = 1;
        this.loadModuleList();
    }
}
