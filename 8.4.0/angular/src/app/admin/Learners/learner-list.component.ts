import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { CreateLearnerComponent } from './create-learner/create-learner.component';
import { EditLearnerComponent } from './edit-learner/edit-learner.component';
import { learnerDto } from '@shared/Dtos/learnerDto'

@Component({
    selector: 'app-learner-list',
    templateUrl: './learner-list.component.html',
})
export class LearnerListComponent implements OnInit {
    public apiUrl = 'https://localhost:44311/api/services/app/Learner';

    learners: learnerDto[] = [];
    currentPagedLearners: any[] = [];
    searchKeyword = '';

    pageNumber = 1;
    pageSize = 5;
    totalItems = 0;

    constructor(
        private http: HttpClient,
        public bsModalRef: BsModalRef,
        private modalService: BsModalService
    ) { }

    ngOnInit(): void {
        this.loadLearnerList();
    }

    public loadLearnerList() {
        this.http.get<any>(`${this.apiUrl}/GetAllLearners`).subscribe({
            next: (response) => {
                this.learners = response.result || [];
                this.totalItems = this.learners.length;
            },
            error: (error) => console.error('Error loading learners!', error)
        });
    }

    public openCreateLearnerDialog() {
        const modalRef = this.modalService.show(CreateLearnerComponent);
        modalRef.content.onSave?.subscribe(() => this.loadLearnerList());
    }

    public EditLearner(learner: learnerDto) {
        const initialState = { learner };
        const modalRef = this.modalService.show(EditLearnerComponent, { initialState });
        modalRef.content.onSave?.subscribe(() => this.loadLearnerList());
    }

    public DeleteLearner(learner: any): void {
        abp.message.confirm(
            `Are you sure you want to delete "${learner.name}"?`,
            'Delete Confirmation'
        ).then((result: boolean) => {
            if (result) {
                this.http.delete(`${this.apiUrl}/DeleteLearner?id=${learner.id}`).subscribe({
                    next: () => {
                        abp.notify.success('Learner deleted successfully!');
                        this.loadLearnerList();
                    },
                    error: (error) => {
                        abp.notify.error('Error deleting learner!');
                        console.error('Error deleting learner!', error);
                    }
                });
            }
        });
    }


    public onSearch(keyword: string) {
        if (!keyword || keyword.trim() === '') {
            this.loadLearnerList();
            return;
        }

        this.http.get<any>(`${this.apiUrl}/GetLearnersByKeyword?keyword=${keyword}`).subscribe({
            next: (response) => {
                this.learners = response.result || [];
                this.totalItems = this.learners.length;
            },
            error: (error) => console.error('Error searching learners!', error)
        });
    }

    public refresh() {
        this.searchKeyword = '';
        this.pageNumber = 1;
        this.loadLearnerList();
    }
}
