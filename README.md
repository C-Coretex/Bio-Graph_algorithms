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
  - `X` - any amino acid
  - `B` - D or E
  - `Z` - N or Q
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
This program finds an **optimal placement of honey supplies** on paths in a forest trail network, modeled as a **connected undirected weighted graph**.

### What the program does

- Reads a **connected undirected graph** representing a trail network:
  - **Vertices** are trail intersections
  - **Edges** are trails between intersections
- Each edge has an associated **integer weight** representing the difficulty of accessing the tree hollow along that trail
- Determines a set of edges on which to place honey supplies such that:
  - **Every cycle in the graph** contains **at least one selected edge**
  - The **sum of weights** of all selected edges is **as small as possible**

The task is to choose a **minimum-weight edge set that breaks all cycles**.
This corresponds to finding a **minimum-weight feedback edge set** in an undirected graph

### Input format

A sequence of integers describing the graph:
  - `n` - number of vertices (`n ≤ 5000`)
  - Followed by an arbitrary number of triples `(a, b, w)`:
    - `a`, `b` - endpoints of an edge (`1 … n`)
    - `w` - edge weight (`-99 … 99`)

### Output

The program produces a **text output file** containing:
- `k` - number of selected edges
- `W` - total weight of selected edges
- A list of `k` vertex pairs `(aᵢ, bᵢ)` identifying the edges where honey supplies are placed