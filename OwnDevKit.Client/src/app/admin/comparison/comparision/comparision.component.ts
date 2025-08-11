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

  constructor(
    private fb: FormBuilder,
    private comparisionService: ComparisionService
  ) {}

  compareCode() {
    const payload = this.compareForm.value;

    this.comparisionService.compareCode(payload).subscribe((res: any) => {
      const oldLines = res.oldCodeLines || [];
      const newLines = res.newCodeLines || [];

      const maxLines = Math.max(oldLines.length, newLines.length);

      this.oldCodeLines = [];
      this.newCodeLines = [];

      for (let i = 0; i < maxLines; i++) {
        const oldLine = oldLines[i] || { segments: [] };
        const newLine = newLines[i] || { segments: [] };
        this.oldCodeLines.push(oldLine);
        this.newCodeLines.push(newLine);
      }

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

  getMaxLines(): number[] {
    const max = Math.max(this.oldCodeLines.length, this.newCodeLines.length);
    return Array.from({ length: max }, (_, i) => i);
  }

  formatText(text: string): string {
    if (!text) return '';
    return text
      .replace(/ /g, '&nbsp;') // preserve spaces
      .replace(/\t/g, '&nbsp;&nbsp;&nbsp;&nbsp;'); // preserve tabs
  }

}