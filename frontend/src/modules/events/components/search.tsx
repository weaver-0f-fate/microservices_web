import React, { useEffect } from 'react';
import { Autocomplete, Box, ListItem, TextField, Typography } from "@mui/material"
import { SearchEvent, useSearchEvents } from '../fetch/useSearchEvents';
import { useDispatch } from 'react-redux';
import { setSelectedEventUuid } from '../store/selectedEventSlice';

interface SearchProps {
    openDialog: () => void
}

const Search = (props: SearchProps) => {
    const searchEvents = useSearchEvents();
    const [searchString, setSearchString] = React.useState<string>('');
    const [options, setOptions] = React.useState<SearchEvent[]>([]);
    const [value, setValue] = React.useState<SearchEvent | null>(null);
    const dispatch = useDispatch();

    useEffect(() => {
        if(searchString){
            searchEvents(searchString)
                .then((data: SearchEvent[]) => {
                    setOptions(data);
                })
                .catch(error => console.log(error));
        }
    }, [searchString])

    const handleSearch = (newValue: string) => {
        setSearchString(newValue);
    }

    const handleChanged = (event: any, newValue: SearchEvent | null) => {
        if(newValue){
            setValue(newValue);
            dispatch(setSelectedEventUuid(newValue ? newValue.uuid : ''))
            props.openDialog();
        }
    }

    return (
        <Box display="flex" sx={{ p: 2, width: '100%', justifyContent: 'center' }}>
            <Autocomplete 
                options={options}
                getOptionLabel={(option: SearchEvent) => option.title}
                filterOptions={(options: SearchEvent[], state: any) => options}
                isOptionEqualToValue={(option: SearchEvent, value: SearchEvent) => {
                    return option.uuid === value.uuid;
                }}
                value={value}
                sx={{ width: 500 }}
                onInputChange={(event: any, newValue: any) => {
                    handleSearch(newValue);
                }}
                onChange={handleChanged}
                renderInput={(params) => (
                    <TextField {...params} label="Search" variant="outlined" fullWidth />
                )}
                renderOption={(props: any, option: SearchEvent) => (
                    <ListItem {...props}>
                        <Box display='flex' sx={{flexDirection: 'column'}}>
                            <Typography>{option.title} {option.place ? `(${option.place})` : ''}</Typography>
                            {option.description ? <Typography variant='body2'>{option.description}</Typography> : ''}                  
                        </Box>
                    </ListItem>
                )}/>

        </Box>
    )
}

export default Search;