import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-learner-list',
  templateUrl: './learner-list.component.html',
//  styleUrls: ['./learner-list.component.css']
})
export class LearnerListComponent implements OnInit {
    public LearnerForm: FormGroup;
    public apiUrl = 'https://localhost:44311/api/services/app/Learner';
    public learners: any[] = [];

    constructor(
        private fb: FormBuilder,
        private http: HttpClient
    ) { }

    ngOnInit(): void {
        this.LearnerForm = this.fb.group({
            id: 0,
            name: ['', [Validators.required, Validators.minLength(3)]],
            email: ['', [Validators.required, Validators.email]],
        })
        this.loadLearnerList();
    }

    get f() {
        return this.LearnerForm.controls;
    }

    public loadLearnerList() {
        debugger;
        this.http.get<any[]>(`${this.apiUrl}/GetAllLearners`)
            .subscribe({
                next: (response: any) => {
                    this.learners = response.result;
                    console.log('Learners loaded successfully:', this.learners);
                },
                error: (error: any) => {
                    console.error('There was an error loading learners!', error);
                }
            })
    }

    Reset() {
        this.LearnerForm.reset();
    }

    Save() {
        debugger;
        if (this.LearnerForm.invalid) {
            return;
        }

        var formData = this.LearnerForm.value;
        console.log(formData);

        if (formData.id == 0) {
            debugger;
            this.http.post(`${this.apiUrl}/CreateLearner`, formData)
                .subscribe({
                    next: (response: any) => {
                        console.log('Learner created successfully!', response);
                        this.LearnerForm.reset();
                        this.loadLearnerList();
                    },
                    error: (error: any) => {
                        console.error('There was an error creating the learner!', error);
                    }
                })
        } else {
            debugger;
            this.http.put(`${this.apiUrl}/UpdateLearner`, formData)
                .subscribe({
                    next: (response: any) => {
                        console.log('Learner updated successfully!', response);
                        this.LearnerForm.reset();
                        this.loadLearnerList();
                    },
                    error: (error: any) => {
                        console.warn('There was an error updating the learner!', error);
                    }
                })
        }
       
    }

    EditLearner(learner: any) {
        debugger;
        this.LearnerForm.patchValue({
            id: learner.id,
            name: learner.name,
            email: learner.email
        })
    }

    DeleteLearner(Learner: any) {
        debugger;
        if (confirm(`Are you sure you want to delete ${Learner.name}?`)) {
            this.http.delete(`${this.apiUrl}/DeleteLearner?id=${Learner.id}`)
                .subscribe({
                    next: (response: any) => {
                        console.warn('Learner deleted successfully!', response);
                        this.loadLearnerList();
                    },
                    error: (error: any) => {
                        console.error('There was an error deleting the learner!', error);
                    }
                })
        }
    }



}
