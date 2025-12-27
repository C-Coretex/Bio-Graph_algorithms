# Bio task description summary
This program computes the **optimal local alignment** of two protein sequences using the **Smith–Waterman algorithm**.

### What the program does

- Reads **two protein sequences**, each from a separate **FASTA file**
- Reads a **similarity matrix** (e.g. BLOSUM62) in the standard FASTA/SSearch format
- Optionally accepts **affine gap penalties** (gap opening and gap extension) as command-line arguments
- Applies the **Smith–Waterman dynamic programming algorithm** to:
  - find the best local alignment
  - compute the corresponding alignment score

### Supported data

- Input sequences must use the **20 standard amino acids**  
  `(A, R, N, D, C, Q, E, G, H, I, L, K, M, F, P, S, T, W, Y, V)`
- The similarity matrix may include an extended alphabet:
  - `X` – any amino acid
  - `B` – D or E
  - `Z` – N or Q
- The program does not need to recognize and process as valid FASTA input sequences the ones, which contain any other symbols, apart from the 20 amino acids

### Output

The program produces a **text output file** containing:
- Both input protein sequences
- The name of the similarity matrix used
- The gap penalty values
- The optimal local alignment score
- The aligned sequences in a readable format


---
# Graph task description summary
...