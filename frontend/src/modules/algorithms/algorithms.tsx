import { Box, Typography } from "@mui/material"
import { useEffect, useState } from "react";
import { AlgorithmResponse, useTestAlgorithms } from "./fetch/useTestAlgorithm";
import { DataGrid, GridColDef } from '@mui/x-data-grid';


const Algorithms = () => {

    const [rows, setRows] = useState<AlgorithmResponse[]>([]);
    const testAlgorithms = useTestAlgorithms();

    useEffect(() => {
        testAlgorithms()
            .then(result => setRows(result.map((algorithm: AlgorithmResponse) => {
                return {
                    uuid: algorithm.uuid,
                    name: algorithm.name
                }
            })));
    })

    const columns: GridColDef[] = [
        { field: 'uuid', headerName: 'ID', resizable: false},
        { field: 'name', headerName: 'Algorithm name'}
    ];

    return (
        <Box sx={{ width: '100%', display: 'flex', justifyContent: 'center' }}>
            <Box sx={{ maxWidth: '50%'}}>
                <DataGrid
                    rows={rows}
                    getRowId={(row) => row.uuid}
                    columnVisibilityModel={{ uuid: false }}
                    columns={columns}
                    initialState={{
                        pagination: {
                        paginationModel: { page: 0, pageSize: 5 },
                        },
                    }}
                    pageSizeOptions={[5, 10]}
                    />
            </Box>
        </Box>
    )
}

export default Algorithms;