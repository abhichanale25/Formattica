import { Component } from '@angular/core';
import { ComparisionService } from '../../services/comparison.service';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-comparision',
  templateUrl: './comparision.component.html',
  styleUrls: ['./comparision.component.css']
})
export class ComparisionComponent {
  compareForm = this.fb.group({
    oldCode: [''],
    newCode: ['']
  });
  
  oldCodeLines: any[] = [];
  newCodeLines: any[] = [];
  comparisonDone = false;
  
  constructor(private fb: FormBuilder, private comparisionService: ComparisionService) {}
  
  compareCode() {
    const payload = this.compareForm.value;
  
    this.comparisionService.compareCode(payload).subscribe((res: any) => {
      this.oldCodeLines = res.oldCodeLines;
      this.newCodeLines = res.newCodeLines;
      this.comparisonDone = true;
    });
  }
  
  clear() {
    this.compareForm.reset();
    this.oldCodeLines = [];
    this.newCodeLines = [];
    this.comparisonDone = false;
  }
  
  get hasTextToClear(): boolean {
    return !!this.compareForm.get('oldCode')?.value || !!this.compareForm.get('newCode')?.value;
  }
  
}