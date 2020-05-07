﻿using CustomerTestsExcel.Indentation;
using System;
using System.Collections.Generic;

namespace CustomerTestsExcel.ExcelToCode
{
    public class ExcelState
    {
        public ITabularPage worksheet;
        public uint row;
        public uint column;

        public ExcelState()
        {
        }

        internal void Initialise(ITabularPage worksheet)
        {
            this.worksheet = worksheet;
            row = 1;
            column = 1;
        }

        public bool RowToColumnIsEmpty(uint column)
        {
            for (uint columnToCheck = 1; columnToCheck <= column; columnToCheck++)
            {
                if (!string.IsNullOrWhiteSpace(Cell(row, columnToCheck))) return false;
            }

            return true;
        }

        public bool RowToCurrentColumnIsEmpty() =>
            RowToColumnIsEmpty(column);

        public bool AllColumnsAreEmpty() =>
            RowToColumnIsEmpty(GetLastColumn());

        public bool AnyPrecedingColumnHasAValue() =>
            !RowToColumnIsEmpty(column - 1);

        public bool AnyFollowingColumnHasAValue(int rowOffset = 0)
        {
            for (uint columnToCheck = column + 1; columnToCheck <
                GetLastColumn(); columnToCheck++)
            {
                if (!string.IsNullOrWhiteSpace(Cell((uint)(row + rowOffset), columnToCheck))) return true;
            }

            return false;
        }

        public void MoveUp(uint by = 1) =>
            row -= by;

        public void MoveDown(uint by = 1) =>
            row += by;

        public void MoveDownToToken(string token)
        {
            while (CurrentCell() != token)
            {
                if (row > GetLastRow()) throw new ExcelToCodeException(string.Format("Cannot find token {0} in column {1}, reached last row ({2})", token, column, row));
                MoveDown();
            }
        }

        public void MoveRight(uint by = 1) =>
            column += by;

        public void MoveLeft(uint by = 1) =>
            column -= by;

        public uint? FindTokenInCurrentRowFromCurrentColumn(string token)
        {
            uint columnToCheck = column;
            while (Cell(row, columnToCheck) != token)
            {
                if (columnToCheck > GetLastColumn()) return null;
                columnToCheck++;
            }

            return columnToCheck;
        }

        public TidyUp SavePosition()
        {
            uint savedRow = this.row;
            uint savedColumn = column;

            return new TidyUp(() => { row = savedRow; column = savedColumn; });
        }

        public TidyUp AutoRestoreMoveDown(uint by = 1)
        {
            MoveDown(by);
            return new TidyUp(() => MoveUp(by));
        }

        public TidyUp AutoRestoreMoveRight(uint by = 1)
        {
            MoveRight(by);
            return new TidyUp(() => MoveLeft(by));
        }

        public TidyUp AutoRestoreMoveDownRight(uint downBy = 1, uint rightBy = 1)
        {
            MoveRight(rightBy);
            MoveDown(downBy);

            return new TidyUp(() =>
            {
                MoveLeft(rightBy);
                MoveUp(downBy);
            });
        }

        public uint GetLastRow() =>
            worksheet.MaxRow;

        public uint GetLastColumn() =>
            worksheet.MaxColumn + 20; // maxcolumn seems to underreport the amount of columns that there are ...

        public string PeekAbove(uint by = 1) =>
            Cell(row - by, column);

        public string PeekBelow(uint by = 1) =>
            Cell(row + by, column);

        public string PeekRight(uint by = 1) =>
            Cell(row, column + by);

        public string PeekBelowRight(uint belowBy = 1, uint rightBy = 1) =>
            Cell(row + belowBy, column + rightBy);

        public string CurrentCell() =>
            Cell(row, column);

        public object CurrentCellRaw() =>
            CellRaw(row, column);

        public string Cell(uint row, uint column)
        {
            var value = worksheet.GetCell(row, column).Value;

            return (value == null) ? "" : value.ToString();
        }

        public object CellRaw(uint row, uint column) =>
            worksheet.GetCell(row, column).Value;

        public string CellReferenceA1Style() =>
            CellReferenceA1Style(row, column);

        public string CellReferenceA1Style(uint row, uint column) =>
            $"{ColumnReferenceA1Style(column)}{row}";

        public string ColumnReferenceA1Style()
            => ColumnReferenceA1Style(column);

        public string ColumnReferenceA1Style(uint column)
        {
            const uint A = 65;
            const uint NUMBER_OF_LETTERS_IN_ALPHABET = 26;
            uint dividend = column;
            string columnName = String.Empty;
            uint modulo;

            // this works because the int representation of all capital letters starts at 65, is continuous and in alphabetical order
            while (dividend > 0)
            {
                modulo = (dividend - 1) % NUMBER_OF_LETTERS_IN_ALPHABET;
                columnName = Convert.ToChar(A + modulo).ToString() + columnName;
                dividend = (dividend - modulo) / NUMBER_OF_LETTERS_IN_ALPHABET;
            }

            return columnName;
        }
    }
}
