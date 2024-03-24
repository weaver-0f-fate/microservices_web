import React, { useEffect } from "react"
import useSetTimeFilter from "../../store/hooks/filters/useSetTimeFilter";
import { Grid, InputLabel } from "@mui/material";
import { TimePicker } from "@mui/x-date-pickers";
import dayjs, { Dayjs } from "dayjs";
import useEvent from "../../store/hooks/useEvent";

const TimeFilter = () => {
    const setTimeFilter = useSetTimeFilter();
    const eventStore = useEvent();
    const [value, setValue] = React.useState<Dayjs | null>(dayjs());

    useEffect(() => {
        if(eventStore.filters.time)
            setValue(dayjs(eventStore.filters.time));
        else
            setValue(null);
    }, [eventStore.filters.time])

    const handleChange = (e: any) => {
        if(e)
            setTimeFilter(e.toDate());
    }

    return (
        <Grid item xs={12}>
            <InputLabel component={TimePicker} value={value} onChange={handleChange} sx={{m:1}} />
        </Grid>
    )
}

export default TimeFilter;