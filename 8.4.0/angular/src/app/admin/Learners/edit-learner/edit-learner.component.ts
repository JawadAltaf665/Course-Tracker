import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { learnerDto } from '../../../../shared/Dtos/learnerDto';

@Component({
  selector: 'app-edit-learner',
  templateUrl: './edit-learner.component.html',
//  styleUrls: ['./edit-learner.component.css']
})
export class EditLearnerComponent implements OnInit {
    public learnerForm: FormGroup;
    public apiUrl = 'https://localhost:44311/api/services/app/Learner';
    public learners: any[] = [];
    @Input() learner!: learnerDto; 

    public onSave: EventEmitter<void> = new EventEmitter<void>();

    constructor(
        private fb: FormBuilder,
        private http: HttpClient,
        public bsModalRef: BsModalRef
    ) { }
    ngOnInit() {
        this.learnerForm = this.fb.group({
            id: 0,
            name: ['', [Validators.required, Validators.minLength(3)]],
            email: ['', [Validators.required, Validators.email]],
        })
        this.bindModuleData();
    }

    public bindModuleData() {
        this.learnerForm.setValue({
            id: this.learner.id,
            name: this.learner.name,
            email: this.learner.email || ''
        })
    }
    get f() {
        return this.learnerForm.controls;
    }

    Reset() {
        this.learnerForm.reset();
    }

    saveLearner() {
        if (this.learnerForm.invalid) {
            return;
        }
        var formData = this.learnerForm.value;
        console.log(formData);

        this.http.put(`${this.apiUrl}/UpdateLearner`, formData)
            .subscribe({
                next: (response: any) => {
                    console.warn('Learner updated successfully!', response);
                    abp.notify.success('Learner updated successfully!');
                    this.onSave.emit();

                    this.bsModalRef.hide();
                },
                error: (error: any) => {
                    console.error('Error updating learner!', error);
                    abp.notify.error('Error updating learner!');
                }
            })
    }
}
