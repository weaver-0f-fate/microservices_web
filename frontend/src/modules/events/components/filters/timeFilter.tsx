import React, { useEffect } from "react"
import { Grid, InputLabel } from "@mui/material";
import { TimePicker } from "@mui/x-date-pickers";
import dayjs, { Dayjs } from "dayjs";
import { Filters, setTimeFilter } from "../../store/filtersSlice";
import { useDispatch, useSelector } from "react-redux";

const TimeFilter = () => {
    const filters = useSelector((state: any) => state.filters as Filters);
    const dispatch = useDispatch();
    const [value, setValue] = React.useState<Dayjs | null>(dayjs());

    useEffect(() => {
        if(filters.time)
            setValue(dayjs(filters.time));
        else
            setValue(null);
    }, [filters.time])

    const handleChange = (e: any) => {
        if(e) 
            dispatch(setTimeFilter(e.toDate()));    
    }

    return (
        <Grid item xs={12}>
            <InputLabel component={TimePicker} value={value} onChange={handleChange} sx={{m:1}} />
        </Grid>
    )
}

export default TimeFilter;